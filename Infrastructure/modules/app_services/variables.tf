variable "location" {
  type        = string
  description = "Location where all our resources are located"
}

variable "app_service_name" {
  type        = string
  description = "Name of the application service"
}

variable "resource_group_name" {
  type        = string
  description = "Name of the resource group"
}

variable "os_type" {
  type        = string
  description = "Operating system type"
}

variable "sku_name" {
  type        = string
  description = "SKU name"
}

variable "web_app_name" {
  type        = string
  description = "Name of the web application"
}

variable "website_port" {
  type        = number
  description = "Port number that expouse on container"
}

variable "docker_image_for_web_app" {
  type        = string
  description = "Docker image for the web application"
}

variable "docker_image_regestry_for_web_app" {
  type        = string
  description = "Docker image registry for the web application"
}

variable "docker_registry_username" {
  type        = string
  sensitive   = true
  description = "Docker image regestry login"
}

variable "docker_registry_password" {
  type        = string
  sensitive   = true
  description = "Docker image regestry password"
}