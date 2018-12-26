import React from 'react';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom';
import Nav from './components/NavBar';
// add navigation

const AppRouter = () => (
    <Router>
        <div>
            <Nav />
            {/* <Route path="/" exact component={Index} />
            <Route path="/about/" component={About} />
            <Route path="/users/" component={Users} /> */}
        </div>
    </Router>
);

export default AppRouter;