
// The zone that has sufficient ECS instances of the required specifications. 
data "alicloud_zones" "default" {
  available_instance_type = data.alicloud_instance_types.default.instance_types[0].id
}

# The ECS instance specifications of the worker nodes. Terraform searches for ECS instance types that fulfill the CPU and memory requests. 
data "alicloud_instance_types" "default" {
  cpu_core_count       = 8
  memory_size          = 32
  availability_zone    = var.availability_zone[0]
  kubernetes_node_role = "Worker"
}

// Check whether the roles are already assigned. 
data "alicloud_ram_roles" "roles" {
    policy_type = "System"
}