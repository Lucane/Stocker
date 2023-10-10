## Description

A stock aggregrator for keeping track of stock statuses from multiple wholesalers.

Also includes spare parts lookup for Lenovo devices (stocks, prices, images).
Spare parts can be searched either by model name or serial number (in which case the original configuration can be viewed).

## Architecture

Blazor Server, SQL Express; hosted with IIS.

Handles API connections to 3 different wholesalers and 2 API connections for spare parts lookup.

## Wholesaler stock
![Screenshot 2023-10-10 184006](https://github.com/Lucane/Stocker/assets/7999446/a6a04494-dbdd-4f06-9e55-7a08539cdb91)


## Spare parts lookup
![Screenshot 2023-10-10 192941](https://github.com/Lucane/Stocker/assets/7999446/3281413b-27e2-4dfd-91e7-04fceafdc034)

![Screenshot 2023-10-10 193046](https://github.com/Lucane/Stocker/assets/7999446/32184e17-23bd-434f-b746-75880491da77)

![Screenshot 2023-10-10 190527](https://github.com/Lucane/Stocker/assets/7999446/c43f4477-1ad5-499b-82bc-ad53499321be)
