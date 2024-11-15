terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.7.0"
    }
  }
  required_version = "1.9.8"

  backend "azurerm" {
    resource_group_name  = "storage"
    storage_account_name = "liubfiles"
    container_name       = "bestrong-tfstate"
    key                  = "task3.tfstate"
    use_oidc             = true
  }
}
provider "azurerm" {
  features {
    key_vault {
      purge_soft_delete_on_destroy    = false
      recover_soft_deleted_key_vaults = true
    }
  }
  # subscription_id = "3616b9f9-5e04-488f-87ac-d012d9d36a69" # test
  # skip_provider_registration = true      # Legacy. Replaced by resource_provider_registrations
  resource_provider_registrations = "core" # https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs#resource-provider-registrations
  use_oidc                        = true
}

resource "azurerm_resource_group" "BeStrong" {
  name     = var.res_group_name
  location = var.res_group_location
}