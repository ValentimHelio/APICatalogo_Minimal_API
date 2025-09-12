namespace APICatalogo.AppServicesExtensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExcptionHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        return app;
    }

    public static IApplicationBuilder UserAppCors(this IApplicationBuilder app)
    {
        app.UseCors(p =>
        {
            p.AllowAnyOrigin();
            p.WithMethods("GET");
            p.AllowAnyHeader();
        });
        return app;
    }

    public static IApplicationBuilder UseSwaggerMiddlewre(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}
