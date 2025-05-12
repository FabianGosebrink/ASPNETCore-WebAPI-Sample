variable "resource_group_name" {
  default = "rg-bestrong"
}

variable "location" {
  default = "East US"
}

variable "aks_name" {
  default = "aks-bestrong"
}

variable "node_count" {
  default = 2
}

variable "subscription_id" {
  description = "Existing Subscription Id"
  type        = string
  sensitive   = true
}