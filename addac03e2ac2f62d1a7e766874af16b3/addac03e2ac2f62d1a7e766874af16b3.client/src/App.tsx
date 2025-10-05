import { useRef, useState } from 'react'
import axios from 'axios'

interface City 
{
    id: number
    name: string
    state: string
    country: string
    rating: number
    dateEstablished: string
    population: number
}

const App = () => 
{
    const cityName = useRef<HTMLInputElement>(null)
    const [cities, setCities] = useState<City[]>([])
    const [loading, setLoading] = useState(false)
    const [weatherLoading, setWeatherLoading] = useState(true)
    const [weatherData, setWeatherData] = useState({count: 0})
    const [error, setError] = useState<string | null>(null)

    const handleSearch = async () => 
    {
        if (!cityName.current) return
        else
        {
            setLoading(true)
            setWeatherLoading(true)
            setError(null)
            axios.get<City[]>(`/api/City/search?name=${cityName.current.value}`).then(r =>
            {
                setLoading(false)
                setCities(r.data)
            })
            .then(() =>
            {
                cityName.current && axios.get(`https://api.openweathermap.org/data/2.5/find?q=${cityName.current.value}&appid=5796abbde9106b7da4febfae8c44c232&units=metric`).
                then(r =>
                {
                    setWeatherData(r.data)
                    setWeatherLoading(false)
                })
            })
            .catch(err =>
            {
                setLoading(false)
                setError(err.response?.data.message|| 'Error fetching cities')
                setCities([])
            })
        }
    }

    return <div className="container mt-4">
        <h1>Search Cities</h1>
        <div className="input-group mb-3">
        <input
            type="text"
            className="form-control"
            placeholder="Enter city name"
            ref={cityName}
            onKeyDown={e => e.key === 'Enter' && handleSearch()}
        />
        <button className="btn btn-primary" onClick={handleSearch} disabled={loading}>
            {loading ? 'Searching...' : 'Search'}
        </button>
        </div>

        {
            error && <div className="alert alert-danger">{error} Would you like to search Restcountries API for this city?
                <button 
                    type="button" 
                    className="btn btn-success" 
                    onClick={() => axios.get(`/api/city/restcountries?cityName=${cityName.current?.value}`).then(r => setWeatherData(r.data)).catch(e => setError(e.response?.data.message))}
                >Yes</button>
                </div>
        }

        {
            cities.length > 0 && <>
                <table className="table table-bordered">
                    <thead className="table-light">
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>State</th>
                        <th>Country</th>
                        <th>Tourist Rating</th>
                        <th>Date Established</th>
                        <th>Population</th>
                    </tr>
                    </thead>
                    <tbody>
                    {
                        cities.map
                        (
                            city => 
                            (
                                <tr key={city.id}>
                                    <td>{city.id}</td>
                                    <td>{city.name}</td>
                                    <td>{city.state}</td>
                                    <td>{city.country}</td>
                                    <td>{city.rating}</td>
                                    <td>{new Date(city.dateEstablished).toLocaleDateString()}</td>
                                    <td>{city.population.toLocaleString()}</td>
                                </tr>
                            )
                        )
                    }
                    </tbody>
                </table>
                {weatherLoading && <div>Loading weather data...</div>}
                {!weatherLoading && weatherData.count > 0 && <div>Weather data:<pre>{JSON.stringify(weatherData, null, 2)}</pre></div>}
                {cityName.current && weatherData.count === 0 && <div>No weather data found for {cityName.current.value}</div>}
            </>
        }
    </div>
}
export default App
