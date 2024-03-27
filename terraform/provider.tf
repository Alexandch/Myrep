terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }
  }
  required_version = ">= 0.13"
}

provider "yandex" {
  token     = var.YANDEX_TOKEN
  cloud_id  = local.cloud_id
  folder_id = local.folder_id
  zone      = local.zone
}

locals {
  cloud_id  = "b1g4o7b6kq75vt9db9ec"
  folder_id = "b1g4kn2sf0lbhnjnl99p"
  zone      = "ru-central1-b"
}

