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
    const [error, setError] = useState<string | null>(null)

    const handleSearch = async () => 
    {
        if (!cityName.current) return
        else
        {
            setLoading(true)
            setError(null)
            axios.get<City[]>(`/api/City/search?name=${cityName.current.value}`).then(r =>
            {
                setLoading(false)
                setCities(r.data)
            })
            .catch(err =>
            {
                setLoading(false)
                setError(err.response?.statusText || 'Error fetching cities')
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

        {error && <div className="alert alert-danger">{error}</div>}

        {
            cities.length > 0 && 
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
        }
    </div>
}
export default App
