namespace AADDemo.API2
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Authentication.JwtBearer;
open Microsoft.IdentityModel.Logging
open Microsoft.IdentityModel.Tokens

type Startup private () =
  new (configuration: IConfiguration) as this =
    Startup() then
    this.Configuration <- configuration
  
  member _.ConfigureServices(services: IServiceCollection) =
    // usefull when debugging token problems. Remove in production
    // https://bartwullems.blogspot.com/2019/04/pii-is-hidden.html
    IdentityModelEventSource.ShowPII <- true;
    
    services.AddCors(
      fun p -> 
        p.AddPolicy(
          name = "AllowAllCors",
          configurePolicy = fun builder ->
            builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader() |> ignore)) |> ignore
    
    services
      .AddAuthentication(fun o ->
      o.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
      o.DefaultChallengeScheme    <- JwtBearerDefaults.AuthenticationScheme
      )
      .AddJwtBearer(fun o ->
      o.Authority                 <- "http://localhost:8080/auth/realms/demo"
      o.Audience                  <- "fable-react-client"
      // remove when on ssl!
      o.RequireHttpsMetadata      <- false
      o.TokenValidationParameters <- TokenValidationParameters (ValidAudiences = [|"account"|])
      ) 
      |> ignore
    
    services.AddControllers() |> ignore
    
  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
    if (env.IsDevelopment()) then
        app.UseDeveloperExceptionPage() |> ignore
    
    app.UseRouting()            |> ignore
    app.UseCors("AllowAllCors") |> ignore
    app.UseAuthentication()     |> ignore 
    app.UseAuthorization()      |> ignore
    
    app.UseEndpoints(fun endpoints ->
        endpoints.MapControllers() |> ignore
        ) |> ignore
    
  member val Configuration : IConfiguration = null with get, set

