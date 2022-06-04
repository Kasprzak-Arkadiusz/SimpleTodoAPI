# SimpleTodoAPI
Simple Todo API - a task for recrutation

## Table of contents
* [Requirements](#requirements)
* [Setup](#setup)

## Requirements

To run the Api locally you need:

* `.NET 6`

* `Docker 20.10.14` (optional)
* `Docker-compose  1.29.2` (optional)

## Setup
To setup a project follow these steps:
1. Clone repository from GitHub  

    Now you have two options:  

    a) Run database locally
    
    - Change your directory to SimpleTodoAPI/src/Api/ 
    
    - Change appSettings.json "DbConnectionString" value to connection to your database
    
    - Enter:
   
      ```bash
      dotnet restore
      ```

    - and then:

      ```bash
      dotnet run
      ```
    
    - Open your favorite browser and type in the address bar:

      ```bash
      localhost:7146/swagger/index.html
      ```

    b) Run database in Docker  
    
    - Enter into SimpleTodoAPI/ folder and run in terminal:  
    
      ```sh
      docker-compose up
      ```
    
    - Wait for docker to create the containers.
    
    - Open your favorite browser and type in the address bar:
    
      ```bash
      localhost:5000/swagger/index.html
      ```
