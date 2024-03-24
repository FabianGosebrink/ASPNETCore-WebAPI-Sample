variable "location" {
  type        = string
  description = "Location where all our resources are located"
}

variable "acr_name" {
  type        = string
  description = "Azure  container regestry name "
}

variable "resource_group_name" {
  type        = string
  description = "Name of the resource group"
}

variable "sku_name" {
  type        = string
  description = "SKU name"
}

