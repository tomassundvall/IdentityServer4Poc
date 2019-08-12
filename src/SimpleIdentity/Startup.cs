// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SimpleIdentity
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
            
            services.AddAuthentication()
                .AddGoogle("Google", opt =>
                {
                    opt.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    opt.ClientId = "467686259744-eoi6302ispjvted3bsg654q3f8k723cm.apps.googleusercontent.com";
                    opt.ClientSecret = "iVnD2YyuxXH7T_VlUS_L_e0m";
                })
                .AddOpenIdConnect("oidc", opt =>
                {
                    opt.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    opt.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    opt.SaveTokens = true;

                    opt.Authority = "https://demo.identityserver.io/";
                    opt.ClientId = "implicit";

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to support static files
            app.UseStaticFiles();

            app.UseIdentityServer();

            // uncomment, if you want to add an MVC-based UI
            app.UseMvcWithDefaultRoute();
        }
    }
}
