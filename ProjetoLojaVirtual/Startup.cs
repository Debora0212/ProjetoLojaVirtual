﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using ProjetoLojaVirtual.Database;
using ProjetoLojaVirtual.Repositories.Contracts;
using ProjetoLojaVirtual.Repositories;
using ProjetoLojaVirtual.Libraries.Sessao;
using ProjetoLojaVirtual.Libraries.Login;
using System.Net.Mail;
using System.Net;
using ProjetoLojaVirtual.Libraries.Email;
using ProjetoLojaVirtual.Libraries.Middleware;
using ProjetoLojaVirtual.Libraries.CarrinhoCompra;
using AutoMapper;
using ProjetoLojaVirtual.Libraries.AutoMapper;
using ProjetoLojaVirtual.Libraries.Gerenciador.Frete;
using WSCorreios;
using ProjetoLojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using Coravel;
using ProjetoLojaVirtual.Libraries.Gerenciador.Schedule.Invocable;

namespace ProjetoLojaVirtual
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * API - Logging
             */
            services.AddLogging();
            /*
             * AutoMapper
             */
            services.AddAutoMapper(config=>config.AddProfile<MappingProfile>());

            /*
             * Padrão repository
             */

            services.AddHttpContextAccessor();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<INewsletterRepository, NewsletterRepository>();
            services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IImagemRepository, ImagemRepository>();
            services.AddScoped<IEnderecoEntregaRepository, EnderecoEntregaRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoSituacaoRepository, PedidoSituacaoRepository>();

            /*
             *SMTP 
             */
            services.AddScoped<SmtpClient>(options => {
                SmtpClient smtp = new SmtpClient()
                {
                    Host = Configuration.GetValue<string>("Email:ServerSMTP"),
                    Port = Configuration.GetValue<int>("Email:ServerPort"),
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Configuration.GetValue<string>("Email:Username"), Configuration.GetValue<string>("Email:Password")),
                    EnableSsl = true
                };

                return smtp;
            });
            services.AddScoped<CalcPrecoPrazoWSSoap>(options => {
                var servico = new CalcPrecoPrazoWSSoapClient(CalcPrecoPrazoWSSoapClient.EndpointConfiguration.CalcPrecoPrazoWSSoap);
                return servico;
            });
            services.AddScoped<GerenciarEmail>();
            services.AddScoped<ProjetoLojaVirtual.Libraries.Cookie.Cookie>();
            services.AddScoped<CookieCarrinhoCompra>();
            services.AddScoped<CookieFrete>();
            services.AddScoped<CalcularPacote>();
            services.AddScoped<WSCorreiosCalcularFrete>();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            /*
             * Session configuration
             */
            services.AddMemoryCache(); //Guardar os dados na memória
           

            services.AddScoped<Sessao>();
            services.AddScoped<ProjetoLojaVirtual.Libraries.Cookie.Cookie>();
            services.AddScoped<LoginCliente>();
            services.AddScoped<LoginColaborador>();
            services.AddScoped<GerenciarPagarMe>();

            services.AddMvc(options=>{
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "O campo deve ser preenchido!");
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddSessionStateTempDataProvider();

            services.AddSession(Options =>
            {
                Options.Cookie.IsEssential = true;
            });

            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaVirtual;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            services.AddDbContext<LojaVirtualContext>(Options => Options.UseSqlServer(connection));


            services.AddTransient<PedidoPagamentoSituacaoJob>();
            services.AddTransient<PedidoEntregueJob>();
            services.AddTransient<PedidoFinalizadoJob>();
            services.AddTransient<PedidoDevolverEntregueJob>();
            services.AddScheduler();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())       
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error/Error500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
             );
                routes.MapRoute(
                    name: "default",
                    template: "/{controller=Home}/{action=Index}/{id?}");
            });

            /*
             * Scheduler - Coravel.
             */

            app.ApplicationServices.UseScheduler(scheduler =>
            {
                scheduler.Schedule<PedidoPagamentoSituacaoJob>().EveryTenSeconds();
                scheduler.Schedule<PedidoEntregueJob>().EveryTenSeconds();
                scheduler.Schedule<PedidoFinalizadoJob>().EveryTenSeconds();
                scheduler.Schedule<PedidoDevolverEntregueJob>().EveryTenSeconds();

            });
        }
    }
}
