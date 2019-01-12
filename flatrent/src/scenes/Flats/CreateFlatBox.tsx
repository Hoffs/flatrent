import React from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import Button from "../../components/Button";
import Card from "../../components/Card";
import UserService, { Policies } from "../../services/UserService";
import Styles from "./CreateFlatBox.module.css";

const CreateFlatBox = (props: RouteComponentProps) => {
  const routeTo = () => props.history.push("/flats/create");
  if (!UserService.satisfiesRoles(...Policies.Supply)) {
    return (<></>);
  }

  return (
    <Card className={Styles.card}>
      Sukurti naują butą
      <Button outline={true} onClick={routeTo} className={Styles.button}>
        Sukurti
      </Button>
    </Card>
  );
}

export default withRouter(CreateFlatBox);
