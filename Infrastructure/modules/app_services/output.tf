output "web-app-service-principal_id" {
  value = azurerm_linux_web_app.web-app.identity[0].principal_id
}

output "web-app-public-hostname" {
  value = join("", ["https://", azurerm_linux_web_app.web-app.default_hostname])
}