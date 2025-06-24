import logo from '../src/logo.svg';
// import cat from '../src/cat.png';
import './App.css';
import React from 'react';

console.log(logo);
// import {
//     BrowserRouter as Router,
//     Routes,
//     Route,
//     Link,
// } from 'react-router-dom';

// function LoginPage() {
//     return (
//         <div className="Login">
//             <h2>Login</h2>
//             <form id="loginForm">
//                 <input type="text" id="username" placeholder="Username" required />
//                 <input type="password" id="password" placeholder="Password" required />
//                 <button type="submit">Login</button>
//                 <div id="errorMsg" class="error"></div>
//             </form>
//         </div>

//     );
// }

// function Home() {
//     return (
//         <div className="App">
//             <header className="App-header">
//                 <img src={logo} className="App-logo" alt="logo" />
//                 <p>
//                     Tell me why! Aint nothing but a heart break.
//                 </p>
//                 <p>
//                     Ruby-chan! HAIIIII! Nani gatsuki?
//                 </p>
// 	    	<p>
// 		    Choco Minto youri mo.....
// 	    	</p>
//                 <a
//                     className="App-link"
//                     href="https://reactjs.org"
//                     target="_blank"
//                     rel="noopener noreferrer"
//                 >
//                     Learn React
//                 </a>
//             </header>
//         </div>
//     );
// }

// function App() {
//     return (
//         <Router>
//             <nav>
//                 <Link to="/">Home</Link> | {" "}
//                 <Link to="/login">About</Link>
//             </nav>

//             <Routes>
//                 <Route path="/" element={<Home />} />
//                 <Route path="/login" element={<LoginPage />} />
//             </Routes>

//         </Router>

//     );
// }

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <img src={logo} className="App-logo" alt="logo" />
                <h1>DU DU DU MAX VERSTAPPEN!!</h1>
                <p>
                    Tell me why! Aint nothing but a heart break.
                </p>
                <p>
                    Ruby-chan! HAIIIII! Nani gatsuki?
                </p>
                <p>
                    Choco Minto youri mo.....
                </p>
                <a
                    className="App-link"
                    href="https://reactjs.org"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    Learn React
                </a>
            </header>
        </div>
    );
}

export default App;
