# 📞 PhoneBook Microservices Project

This project is a microservices-based phonebook application developed using .NET. It demonstrates service-to-service communication, asynchronous event-driven architecture, and containerized deployment using Docker.

> This project can be started with a single command using Docker Compose.

---

# 🚀 Features

* Contact management (create, list, delete)
* Contact information management (phone, email, location)
* Soft delete mechanism
* Domain-level validation to ensure data integrity
* Synchronous communication between services (HTTP)
* Asynchronous communication using RabbitMQ
* Separate databases per service
* Fully containerized environment using Docker Compose
* Unit testing for service layer
* Global exception handling middleware
* Basic application logging
* Health check endpoints for service monitoring

---

# 🏗️ Architecture

The system consists of two independent microservices:

## 1. Contact Service

* Manages contacts
* Handles create, list, delete operations
* Publishes events when a contact is deleted

## 2. Contact Info Service

* Manages contact details (phone, email, location)
* Listens to events from Contact Service
* Deletes related contact info asynchronously

---

# 🔄 Communication

## Synchronous (HTTP)

* ContactService → ContactInfoService
* Used for fetching contact details

## Asynchronous (RabbitMQ)

* ContactService publishes `ContactDeletedEvent`
* ContactInfoService consumes the event and removes related data

---

# 🗄️ Database Design

* Each service has its own database (**Database per Service pattern**)
* PostgreSQL is used as the database engine

| Service            | Database Name          |
| ------------------ | ---------------------- |
| ContactService     | PhoneBookContactDb     |
| ContactInfoService | PhoneBookContactInfoDb |

---

# 🛠️ Technologies Used

* .NET 10 (ASP.NET Core)
* PostgreSQL
* RabbitMQ
* Docker & Docker Compose
* xUnit, Moq, FluentAssertions
* Layered architecture inspired by Clean Architecture principles

---

# ▶️ Running the Project

## Prerequisites

* Docker Desktop installed and running

## Run the system

```bash
docker compose up --build
```

---

# 🌐 Endpoints

After running the project:

* Contact Service Swagger:
  http://localhost:5001/swagger

* Contact Info Service Swagger:
  http://localhost:5003/swagger

* RabbitMQ Management UI:
  http://localhost:15672

**Credentials:**
username: guest
password: guest

---

# 🏥 Health Checks

* http://localhost:5001/health
* http://localhost:5003/health

These endpoints indicate whether the services are running and healthy.

---

# 🧪 Example Workflow

1. Create a contact via ContactService
2. Add contact information via ContactInfoService
3. Retrieve contact details
4. Soft delete the contact
5. ContactInfoService automatically deletes related records via RabbitMQ

---

# ⚠️ Error Handling

Global exception middleware is implemented:

| Exception Type       | HTTP Status |
| -------------------- | ----------- |
| KeyNotFoundException | 404         |
| ArgumentException    | 400         |
| Other Exceptions     | 500         |

---

# 🧪 Validation

Validation is implemented at the domain level:

* Contact entity validates required fields (e.g., Name)
* ContactInfo entity validates:

  * Required content
  * Email format (basic validation)
  * Phone number length

This ensures invalid entities cannot be created.

---

# 📊 Logging

Basic logging is implemented using built-in ASP.NET Core logging:

* Exception logs via middleware
* Event publishing logs in RabbitMQ publisher
* Event consumption logs in RabbitMQ consumer

Logs can be viewed via:

```bash
docker logs <container-name>
```

---

# 🧪 Testing

Unit tests are implemented for service layer:

* ContactManager
* ContactInfoManager

Run tests:

```bash
dotnet test
```

---

# 📌 Design Decisions

* Microservices architecture for scalability and separation of concerns
* Database per service pattern to ensure service independence
* RabbitMQ for eventual consistency and asynchronous processing
* HTTP communication for real-time data retrieval
* Soft delete strategy to preserve data integrity
* Domain-level validation for enforcing business rules
* Layered architecture inspired by Clean Architecture principles

---

# 📎 Notes

* Authentication and authorization are not included, as the focus of this project is microservice communication and infrastructure. In a real-world scenario, JWT-based authentication could be implemented.
* Basic logging is implemented; in production, centralized logging (e.g., Serilog, ELK) could be added

---

# 👨‍💻 Author

Developed by Alper Horat as part of a backend assessment project.
