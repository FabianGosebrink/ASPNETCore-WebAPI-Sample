# === Create a Container Registry ===
resource "azurerm_container_registry" "acr" {
  name                = var.container_registry_name
  location            = azurerm_resource_group.BeStrong.location
  resource_group_name = azurerm_resource_group.BeStrong.name
  sku                 = "Basic"
  admin_enabled       = false
}