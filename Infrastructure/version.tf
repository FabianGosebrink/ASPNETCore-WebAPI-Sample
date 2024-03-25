terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.94.0 "
    }
    random = {
      source  = "hashicorp/random"
      version = "~>3.6.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "NetworkWatcherRG"
    storage_account_name = "tfstate123423232"
    container_name       = "tf-tfstate-for-production"
    key                  = "prod.tfstate"
  }
}

provider "azurerm" {
  features {}
}