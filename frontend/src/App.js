import Home from "./pages/Home";
import Login from "./pages/Login";
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";


// console.log(cat);

function App() {
    return (
        <Router>
            <nav>
                <Link to="/">Home</Link> | {" "}
                <Link to="/login">Login</Link>
            </nav>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
            </Routes>
        </Router>
    );
}

export default App;
