<script setup lang="ts">
import { ref } from 'vue'
import type { Ref } from 'vue'
import type { WeatherForecast } from '../types'

const forecastData: Ref<WeatherForecast[]|null> = ref(null)

async function fetchWeatherForecast() {
  forecastData.value = null;
  const results = await fetch('/WeatherForecast/')
  forecastData.value = await results.json()
} 
</script>

<template>
  <button @click="fetchWeatherForecast()">Fetch Weather Forecast</button>
  <p v-if="!forecastData">Loading...</p>
  <ul v-else>
    <li v-for="forecast in forecastData" :key="forecast.date">
      Date: {{ forecast.date }}<br/>
      Temperature: {{ forecast.temperatureC }}C ({{ forecast.temperatureF }}F)<br/>
      Summary: {{ forecast.summary || "Nothing" }}
    </li>
  </ul>
</template>

<style>
</style>