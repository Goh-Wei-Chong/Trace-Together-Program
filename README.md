# Programming 2 (PRG2) Assignment 3 - Trace Together Program
C# code on trace together as a final pair assignment

## Objective
This pair assignment is to create a Covid-19 monitoring system that keeps track of users status and their SHN stays. Furthermore, users can implement more stuff
such as monitoring the SHN hotels, viewing visitors to the SHN hotel and creating travel entry records for visitors. 

## Features
The program has 4 main menus in the system:
1. SafeEntry/TraceTogether - The user is able to create or replace the TraceTogether token when necessary, edit business location capacity, and check in 
and check out visitors from locations.
2. TravelEntry - This allows user to create a visitor and a TravelEntry record for them, and also calculate the SHN charges for certain visitors who recently served their 
SHN stay.
3. General - As a user that has admin status, you are allowed to view all visitor, business location and SHN facility details.
4. Advanced - This section is additional features for the assignment. The user can have a contact tracing report, view SHN status reports and check out of all locations 
for visitors.

## API
The program makes use of this [API](https://covidmonitoringapiprg2.azurewebsites.net/facility) to get the details of the SHN facilities.

