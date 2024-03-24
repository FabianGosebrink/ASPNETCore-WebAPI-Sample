

# Service plan with service app declaretion. With additional key vault access policy set up
resource "azurerm_service_plan" "default" {
  name                = var.app_service_name
  location            = var.location
  resource_group_name = var.resource_group_name
  os_type             = var.os_type
  sku_name            = var.sku_name
}

resource "azurerm_linux_web_app" "web-app" {
  name                = var.web_app_name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.default.id
  site_config {
    application_stack {
      docker_registry_username = var.docker_registry_username
      docker_registry_password = var.docker_registry_password
      docker_image_name        = "${var.docker_image_for_web_app}"
      docker_registry_url      = "https://${var.docker_image_regestry_for_web_app}"
    }
  }
  identity {
    type = "SystemAssigned"
  }
  app_settings = {
    WEBSITES_PORT                       = var.website_port # should be 80 by default
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false

  }
}