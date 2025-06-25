import React, { useEffect, useState } from 'react';

function Weather() {
  const [weather, setWeather] = useState(null);

  useEffect(() => {
    fetch('/api/weather') // This gets routed through NGINX to your ASP.NET backend
      .then(res => res.json())
      .then(data => {
        console.log(data);
        setWeather(data);
      })
      .catch(err => console.error('Error fetching weather:', err));
  }, []);

  return (
    <div>
      <h1>Weather Info</h1>
      {weather ? (
        <pre>{JSON.stringify(weather, null, 2)}</pre>
      ) : (
        <p>Loading weather data...</p>
      )}
    </div>
  );
}

export default Weather;