# Present-Sir

The repository that contains the source code for Present Sir, a USSD based class attendance logging system.

Built at the 2018 forLoopGhana UG Hackathon, using Africa's Talking USSD API and Xamarin.Android for the Android app and ASP.NET MVC5 + LiteDB for the backend.

> Contributors: Mbah Clinton, Eyioyo Omatsola

This repository contains:

1. PresentSir.Web - our ASP.NET Backend
2. PresentSir.Droid - our Xamarin.Android app
3. PresentSir - The original Java android app before the rewrite in C# because of the verbosity of Java and our time constraints.

# Functionality

Present Sir is built for students and teachers. Below is more information on the functionality available to both classes of users.

### Teacher

1.  Create or delete classes students can register for and subsequently mark attendance.
2.  Start an attendance marking session. A class marking USSD code will be provided for you to present to students. That USSD code can currently only be accessed via Africa's Talking USSD simulator.
3.  End an attendance marking session after which students will no longer be able to mark attendance for that particular class.
4.  View students that attended a class on any particular day.

> **Students can mark attendance for a class only if the student has registered for that class and if you (Teacher) have started a marking session.**

### Student

1.  Register or unregister from a class.
2.  Mark attendance for a class using the USSD code the teacher of that class will provide when a marking session is started.

# Getting started

## Android app

To begin, you can download the Present Sir android apk via the link below.

_NB: Only runs on Android 4.4 (Kitkat) and above._

> [Download apk file](https://doc-0g-3g-docs.googleusercontent.com/docs/securesc/ha0ro937gcuc7l7deffksulhg5h7mbp1/gskd868nqsuka5iabqp36is0b3a3d2bp/1520762400000/04851421854738192448/*/1XzqR9Dvq62W937nKrzF-pSXbBeGCYAyd?e=download)

### Signing in

There are two demo accounts available.

| Account type | Username       | Password | Index number |
| ------------ | -------------- | -------- | ------------ |
| Teacher      | mclint-teacher | password | N/A          |
| Student      | mclint         | password | 040915010    |

### Screenshots

#### Login

<img src="https://preview.ibb.co/kmRAoS/Screenshot_1520765126.png" alt="Screenshot_1520765126" border="0">

#### Teacher home -- Classes

<img src="https://preview.ibb.co/fRRkoS/Screenshot_1520765243.png" alt="Screenshot_1520765243" border="0">

#### Teacher -- Create a class

<img src="https://preview.ibb.co/kDhfNn/Screenshot_1520765255.png" alt="Screenshot_1520765255" border="0">

#### Teacher -- Mark attendance

<img src="https://preview.ibb.co/dY0BTS/Screenshot_1520765272.png" alt="Screenshot_1520765272" border="0">

#### Teacher -- View class attendance

<img src="https://preview.ibb.co/fuA6v7/Screenshot_1520766705.png" alt="Screenshot_1520766705" border="0">

#### Student home -- Registered classes

<img src="https://preview.ibb.co/ijYn2n/Screenshot_1520765172.png" alt="Screenshot_1520765172" border="0">

#### Student -- Register for a class

<img src="https://preview.ibb.co/fYb72n/Screenshot_1520765182.png" alt="Screenshot_1520765182" border="0">
