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
                    draggable={true}
                    autoClose={5000}
                    position={toast.POSITION.BOTTOM_CENTER}
                    className="toast-container"
                    toastClassName="toast-c"
                    bodyClassName="toast-body"
                    progressClassName="toast-progress"
                    pauseOnFocusLoss={false}
                />
            </div>
        );
    }
}

export default App;
