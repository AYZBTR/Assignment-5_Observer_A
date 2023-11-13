using System;
using System.Collections.Generic;

// Observer pattern interfaces
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

public interface IObserver
{
    void Update(float temperature, float humidity, float pressure);
}

// WeatherData class implementing the Subject interface
public class WeatherData : ISubject
{
    private List<IObserver> observers;
    private float temperature;
    private float humidity;
    private float pressure;

    public WeatherData()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature, humidity, pressure);
        }
    }

    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        this.pressure = pressure;
        MeasurementsChanged();
    }

    private void MeasurementsChanged()
    {
        NotifyObservers();
    }
}

// Display class implementing the Observer interface
public abstract class Display : IObserver
{
    protected float temperature;
    protected float humidity;
    protected float pressure;

    public abstract void Update(float temperature, float humidity, float pressure);
}

// CurrentConditionsDisplay class
public class CurrentConditionsDisplay : Display
{
    public override void Update(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        this.pressure = pressure;

        Console.WriteLine($"Current conditions: {temperature} degrees and {humidity}% humidity");
    }
}

// StatisticsDisplay class
public class StatisticsDisplay : Display
{
    private List<float> temperatures = new List<float>();

    public override void Update(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        this.pressure = pressure;

        temperatures.Add(temperature);

        float avg = temperatures.Average();
        float max = temperatures.Max();
        float min = temperatures.Min();

        Console.WriteLine($"Avg/Max/Min temperature: {avg}/{max}/{min}");
    }
}

// ForecastDisplay class
public class ForecastDisplay : Display
{
    public override void Update(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        this.pressure = pressure;

        // Add your forecasting logic here
        string forecast = "Improving weather on the way!";
        if (temperature < 80)
        {
            forecast = "Watch out for cooler, rainy weather";
        }
        else if (humidity > 85)
        {
            forecast = "More of the same";
        }

        Console.WriteLine($"Forecast: {forecast}");
    }
}

// Program class
class Program
{
    static void Main()
    {
        WeatherData weatherData = new WeatherData();

        CurrentConditionsDisplay currentConditionsDisplay = new CurrentConditionsDisplay();
        StatisticsDisplay statisticsDisplay = new StatisticsDisplay();
        ForecastDisplay forecastDisplay = new ForecastDisplay();

        weatherData.RegisterObserver(currentConditionsDisplay);
        weatherData.RegisterObserver(statisticsDisplay);
        weatherData.RegisterObserver(forecastDisplay);

        // Simulate new weather measurements
        weatherData.SetMeasurements(80.0f, 65.0f, 1010.0f);
        weatherData.SetMeasurements(82.0f, 70.0f, 1012.0f);
        weatherData.SetMeasurements(78.0f, 90.0f, 1008.0f);
    }
}
