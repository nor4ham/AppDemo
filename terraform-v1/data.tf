data "alicloud_ack_service" "open" {
    enable = "On"
    type   = "propayasgo"
}
// Check whether the roles are already assigned. 
data "alicloud_ram_roles" "roles" {
    policy_type = "System"
}
