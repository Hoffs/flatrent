import React, { Component } from 'react';
import { BrowserRouter, Route, Link } from 'react-router-dom';
import AppRouter from './AppRouter'
import './App.css';

class App extends Component {
  render() {
    return (
      <div className="App">
        <AppRouter />
      </div>
    );
  }
}

export default App;
