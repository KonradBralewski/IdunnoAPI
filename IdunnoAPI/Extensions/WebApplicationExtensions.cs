namespace IdunnoAPI.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void Configure(this WebApplication app) 
        {
            // Configure the HTTP request pipeline.

            app.UseExceptionHandler("/error");

            if (app.Environment.IsDevelopment())
            {
                app.UseCors(ServiceCollectionExtensions.policyName);
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
