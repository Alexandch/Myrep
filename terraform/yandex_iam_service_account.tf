resource "yandex_iam_service_account" "sa" {
  name      = "common-sa-account"
  folder_id = local.folder_id
}
// Grant permissions
resource "yandex_resourcemanager_folder_iam_member" "sa-admin" {
  folder_id = local.folder_id
  role      = "admin"
  member    = "serviceAccount:${yandex_iam_service_account.sa.id}"
}

resource "yandex_resourcemanager_folder_iam_member" "sa-storage-editor" {
  folder_id = local.folder_id
  role      = "storage.editor"
  member    = "serviceAccount:${yandex_iam_service_account.sa.id}"
}

resource "yandex_resourcemanager_folder_iam_member" "sa-storage-uploader" {
  folder_id = local.folder_id
  role      = "storage.uploader"
  member    = "serviceAccount:${yandex_iam_service_account.sa.id}"
}

resource "yandex_resourcemanager_folder_iam_member" "sa-storage-viewer" {
  folder_id = local.folder_id
  role      = "storage.viewer"
  member    = "serviceAccount:${yandex_iam_service_account.sa.id}"
}

resource "yandex_iam_service_account_static_access_key" "sa-terraform-key" {
  service_account_id = yandex_iam_service_account.sa.id
  description        = "static access key for object storage"
}
output "sa_terraform_access_key" {
  value = yandex_iam_service_account_static_access_key.sa-terraform-key.access_key
}
output "sa_terraform_secret_key" {
  value     = yandex_iam_service_account_static_access_key.sa-terraform-key.secret_key
  sensitive = true
}
