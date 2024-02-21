resource "alicloud_security_group" "group" {
  name   = "tf_group"
  vpc_id = alicloud_vpc.defaul.id
}
resource "alicloud_security_group_rule" "allow_all_tcp" {
  type              = "ingress"
  ip_protocol       = "tcp"
  nic_type          = "intranet"
  policy            = "accept"
  port_range        = "1/65535"
  priority          = 1
  security_group_id = alicloud_security_group.group.id
  cidr_ip           = "0.0.0.0/0"
}


/*
resource "alicloud_security_group" "test-sg" {
  name = "test-sg"
  vpc_id      = "${alicloud_vpc.test-vpc.id}"
}

resource "alicloud_security_group_rule" "allow_all_tcp" {
  type              = "ingress"
  ip_protocol       = "tcp"
  nic_type          = "internet"
  policy            = "accept"
  port_range        = "1/65535"
  priority          = 1
  security_group_id = alicloud_security_group.test-sg.id
  cidr_ip           = "0.0.0.0/0"
}


// Create a role. 
resource "alicloud_ram_role" "role" {
  for_each    = { for r in var.roles : r.name => r }
  name        = each.value.name
  document    = each.value.policy_document
  description = each.value.description
  force       = true
}

// Attach a RAM policy to the role. 
resource "alicloud_ram_role_policy_attachment" "attach" {
  for_each    = { for r in var.roles : r.name => r }
  policy_name = each.value.policy_name
  policy_type = "System"
  role_name   = each.value.name
  depends_on  = [alicloud_ram_role.role]
}

 */