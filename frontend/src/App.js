import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Login from "./pages/About";
import React from 'react';

// console.log(cat);

function App() {
    return (
        <Route>
            <nav>
                <Link to="/">Home</Link> | {" "}
                <Link to="/login">About</Link>
            </nav>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
            </Routes>
        </Route>
    );
}

export default App;
