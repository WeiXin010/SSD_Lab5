import Home from "./pages/Home";
import Login from "./pages/Login";
import Weather from "./pages/Weather";
import Files from "./pages/Files";
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";


// console.log(cat);

function App() {
    return (
        <Router>
            <nav>
                <Link to="/">Home</Link> | {" "}
                <Link to="/login">Login</Link> | {" "}
                <Link to="/weather">Weather Forecast</Link> | {" "}
                <Link to="/files">Files</Link>
            </nav>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/weather" element={<Weather />} />
                <Route path="/files" element={<Files />} />
            </Routes>
        </Router>
    );
}

export default App;
