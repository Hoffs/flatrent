import React, { Component } from "react";
import { Slide, toast, ToastContainer } from "react-toastify";
// tslint:disable-next-line: no-submodule-imports
import "react-toastify/dist/ReactToastify.css";
import "./App.css";
import MainRouter from "./MainRouter";

class App extends Component {
  public render() {
    return (
      <div className="App">
        <MainRouter />
        <ToastContainer
          transition={Slide}
          pauseOnHover={false}
          draggable={false}
          autoClose={5000}
          position={toast.POSITION.BOTTOM_CENTER}
        />
      </div>
    );
  }
}

export default App;
