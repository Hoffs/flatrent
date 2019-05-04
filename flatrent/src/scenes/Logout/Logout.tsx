import React from "react";
import { Redirect } from "react-router-dom";
import { toast } from "react-toastify";
import UserService from "../../services/UserService";

const Logout = () => {
    UserService.logout();
    toast.success("Sėkmingai atsijungėte!", {
        position: toast.POSITION.BOTTOM_CENTER,
    });
    return <Redirect to="/" />;
};

export default Logout;
