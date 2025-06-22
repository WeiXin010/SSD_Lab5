import logo from './logo.svg';
import './App.css';
import React from 'react';
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link,
} from 'react-router-dom';

function LoginPage() {
    return (
        <div className="Login">
            <h2>Login</h2>
            <form id="loginForm">
                <input type="text" id="username" placeholder="Username" required />
                <input type="password" id="password" placeholder="Password" required />
                <button type="submit">Login</button>
                <div id="errorMsg" class="error"></div>
            </form>
        </div>

    );
}

function Home() {
    return (
        <div className="App">
            <header className="App-header">
                <p>
                    Tell me why! Aint nothing but a heart break.
                </p>
                <p>
                    Ruby-chan! HAIIIII! Nani gatsuki?
                </p>
	    	<p>
		    Choco Minto youri mo.....
	    	</p>
            </header>
        </div>
    );
}

function App() {
    return (
        <Router>
            <nav>
                <Link to="/">Home</Link> | {" "}
                <Link to="/login">About</Link>
            </nav>

            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<LoginPage />} />
            </Routes>

        </Router>

    );
}

export default App;
