#provider, use alicloud
provider "alicloud" {
  region =  var.region
  profile ="default" 

}

// Activate ACK. 
data "alicloud_ack_service" "open" {
    enable = "On"
    type   = "propayasgo"
}