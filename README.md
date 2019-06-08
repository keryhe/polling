# polling

A set of interfaces and classes for polling a source for data and sending it to a destination.

# Keryhe.Polling

![Keryhe.Polling](https://img.shields.io/nuget/v/Keryhe.Polling.svg)

## Poller

The IPolling interface implements the IMessageListener interface. To implement your own polling class, you will need to create a class that inherits from the Poller abstract class. this class implements the IPolling interface and provides an abstract method, Poll for you to implement yourself.

```c#
protected abstract List<T> Poll();
```

Poll is called whenever it is time to retrieve data from your chosen source, usually a database.

The constructor of the Poller abstract class accepts as one of its parameters an object of type IDelay.

## IDelay

Implementers of the IDelay interface specify the amount of time to wait if no data is found when querying the source. There are four built in IDelay implementations (wait times are in seconds). Included are the appsettings sections needed to configure the delays:

- **ConstantDelay** - Uses a constant wait time. 
    ```json
    "ConstantOptions": 
    {
        "Interval": 1
    }
    ```
- **ExponentialDelay** - Multiplies the previous wait time by a factor in order to get the next wait time.
    ```json
    "ExponentialOptions": 
    {
        "Factor": 2,
        "MaxWait": 60
    }
    ```
- **FibonacciDelay** - Adds the last two wait times together starting at 1 (based on the fibonacci sequence) to get the next wait time.
    ```json
    "FibonacciOptions": 
    {
        "MaxWait": 60
    }
    ```
- **LinearDelay** - Adds a given value to the wait time.
    ```json
    "LinearOptions": 
    {
        "Increment": 1,
        "MaxWait": 60
    }
    ```

Everything is taken care of by the Poller base class. All you need to do is implement the Poll method.