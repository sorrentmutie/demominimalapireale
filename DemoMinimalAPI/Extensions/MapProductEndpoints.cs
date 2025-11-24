using DemoMinimalAPI.Models;
using Microsoft.FeatureManagement;

namespace DemoMinimalAPI.Extensions;

public static class MapProductsEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(
        this IEndpointRouteBuilder app)
    { 
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/{id:int}/price",
            async (IFeatureManager featureManager, int id) =>
            {
                var product = new MyProduct
                {
                    Id = id,
                    OriginalPrice = 100.0M,
                    DiscountPrice = 100.0M,
                    AlgorithmName = "Standard Pricing Algorithm v1.0"
                };

                var applyNewPrincing = await featureManager
                    .IsEnabledAsync("NewPricingAlgorithm");  
                if(applyNewPrincing)
                {
                    var discountPercentage = 0.15;
                   // AVOID THIS
                    // if (id % 2 == 0) discountPercentage = 0.20;

                    product.DiscountPrice = (decimal) ( 1 - discountPercentage) * product.OriginalPrice;
                    product.AlgorithmName = "New Pricing Algorithm v2.0";
                    return Results.Ok(product);
                }
                else
                {
                    return Results.Ok(product);
                }


            });

        group.MapGet("search", async (IFeatureManager featureManager, string? query) =>
        {
            var hasAdvancedSearch = await featureManager
                .IsEnabledAsync("AdvancedSearch");

            if(hasAdvancedSearch)
            {
                return Results.Ok(new
                {
                    Message = "Advanced search results",
                    Query = query
                });
            } else
            {
                return Results.Ok(new
                {
                    Message = "Starndard search results",
                    Query = query
                });
            }

        });

        group.MapGet("premium", async (IFeatureManager featureManager) => { 
           var isPremiumEnabled = await featureManager
                .IsEnabledAsync("PremiumFeatures");

        });

        return group;
    }
}
