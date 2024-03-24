resource "random_id" "tf-RandomPrefix" {
  byte_length = 4
}

# Azure Container Registry creation
resource "azurerm_container_registry" "acr-default" {
  name                = var.acr_name
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = var.sku_name
  admin_enabled       = true
}