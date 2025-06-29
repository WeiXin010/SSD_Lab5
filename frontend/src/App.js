import Home from "./pages/Home";
import Login from "./pages/Login";
import Weather from "./pages/Weather";
import FileUpload from "./pages/FileUpload";
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
                <Link to="/fileUpload">File Upload</Link>
            </nav>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/weather" element={<Weather />} />
                <Route path="/fileUplaod" element={<FileUpload />} />
            </Routes>
        </Router>
    );
}

export default App;
