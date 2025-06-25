import Home from "./pages/Home";
import Login from "./pages/Login";
import Login from "./pages/Weather";
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";


// console.log(cat);

function App() {
    return (
        <Router>
            <nav>
                <Link to="/">Home</Link> | {" "}
                <Link to="/login">Login</Link>
                <Link to="/weather">Weather Forecast</Link>
            </nav>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/weather" element={<Weather />} />
            </Routes>
        </Router>
    );
}

export default App;
