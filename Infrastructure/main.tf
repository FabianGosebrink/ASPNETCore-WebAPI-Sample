# test 2
locals {
  name_location = replace(lower(var.location), " ", "-")
  env           = "prod"
}

resource "random_id" "tf-RandomPrefix" {
  byte_length = 4
}

resource "azurerm_resource_group" "default-RG" {
  name     = "rg-rsourcegroup-${join("-", [local.env, local.name_location])}-001"
  location = var.location
}

module "azure_container_regestry" {
  source              = "./modules/azure_container_regestry"
  location            = var.location
  resource_group_name = azurerm_resource_group.default-RG.name
  acr_name            = "acrwebappcontreg${join("", [local.env, replace(local.name_location, "-", "")])}001"
  sku_name            = "Basic"
}

module "app_services" {
  source                            = "./modules/app_services"
  depends_on                        = [module.azure_container_regestry]
  location                          = var.location
  resource_group_name               = azurerm_resource_group.default-RG.name
  app_service_name                  = "asp-serverfarm-${join("", [local.env, local.name_location])}-001"
  web_app_name                      = "lwp-website-${join("", [local.env, local.name_location])}-001"
  os_type                           = "Linux"
  sku_name                          = "B1"
  website_port                      = 80
  docker_image_for_web_app          = "asp-image:latest"
  docker_registry_username          = module.azure_container_regestry.acr_admin_username
  docker_registry_password          = module.azure_container_regestry.acr_admin_password
  docker_image_regestry_for_web_app = module.azure_container_regestry.acr_login_link
}

resource "azurerm_role_assignment" "app-service-Pull" {
  scope                = module.azure_container_regestry.acr_id
  role_definition_name = "AcrPull"
  principal_id         = module.app_services.web-app-service-principal_id
}
