package com.example.demo;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

/**
 * Sample Weather Controller
 */
@RestController
@RequestMapping("/api/v1")
@Tag(name = "Weather", description = "Weather forecast APIs")
public class WeatherController {

    private static final String[] SUMMARIES = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private final Random random = new Random();

    @GetMapping("/weatherforecast")
    @Operation(summary = "Get weather forecast", description = "Returns weather forecast for the next 5 days")
    public List<WeatherForecast> getWeatherForecast() {
        return Arrays.asList(
                createForecast(1),
                createForecast(2),
                createForecast(3),
                createForecast(4),
                createForecast(5));
    }

    @GetMapping("/weatherforecast/{days}")
    @Operation(summary = "Get weather forecast for specific days", description = "Returns weather forecast for specified number of days")
    public List<WeatherForecast> getWeatherForecastForDays(
            @Parameter(description = "Number of days to forecast") @PathVariable int days) {
        WeatherForecast[] forecasts = new WeatherForecast[days];
        for (int i = 0; i < days; i++) {
            forecasts[i] = createForecast(i + 1);
        }
        return Arrays.asList(forecasts);
    }

    private WeatherForecast createForecast(int daysFromNow) {
        int temperatureC = random.nextInt(75) - 20;
        return new WeatherForecast(
                LocalDate.now().plusDays(daysFromNow),
                temperatureC,
                SUMMARIES[random.nextInt(SUMMARIES.length)]);
    }

    public static class WeatherForecast {
        private LocalDate date;
        private int temperatureC;
        private String summary;

        public WeatherForecast(LocalDate date, int temperatureC, String summary) {
            this.date = date;
            this.temperatureC = temperatureC;
            this.summary = summary;
        }

        public LocalDate getDate() {
            return date;
        }

        public int getTemperatureC() {
            return temperatureC;
        }

        public int getTemperatureF() {
            return 32 + (int) (temperatureC / 0.5556);
        }

        public String getSummary() {
            return summary;
        }
    }
}
