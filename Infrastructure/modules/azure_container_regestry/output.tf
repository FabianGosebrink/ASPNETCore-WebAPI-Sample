output "acr_login_link" {
  value = azurerm_container_registry.acr-default.login_server
}

output "acr_id" {
  value = azurerm_container_registry.acr-default.id
}

output "acr_admin_username" {
  value = azurerm_container_registry.acr-default.admin_username
  sensitive = true
}

output "acr_admin_password" {
  value = azurerm_container_registry.acr-default.admin_password
  sensitive = true
}