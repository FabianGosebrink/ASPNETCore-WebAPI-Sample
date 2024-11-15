# === Create a App Service ===
# Warn: App Service Identity has access to Container Registry: BeStrongContainerRegistry
#       more in containerRegistry.tf
resource "azurerm_service_plan" "svcplan" {
  name                = "BeStrong-appserviceplan"
  location            = azurerm_resource_group.BeStrong.location
  resource_group_name = azurerm_resource_group.BeStrong.name
  os_type             = "Linux"
  sku_name            = "B1"
}

resource "azurerm_linux_web_app" "appsvc" {
  name                = var.web_app_name
  location            = azurerm_resource_group.BeStrong.location
  resource_group_name = azurerm_resource_group.BeStrong.name
  service_plan_id     = azurerm_service_plan.svcplan.id
  site_config {
    always_on = false
  }
  app_settings = {
    ASPNETCORE_URLS = "http://*:80"
    ASPNETCORE_ENVIRONMENT = "Development"
  }

}