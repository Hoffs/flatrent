import React, { Component, ReactNode } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import FlatService, { IFlatInfo, IFlatListResponse } from "../../services/FlatService";
import CreateFlatBox from "./FlatCreateBox";
import FlatItem from "./FlatItem";
import Card from "../../components/Card";
import FlatFilters from "./FlatFilters";

class FlatList extends Component<
  RouteComponentProps,
  { pageSize: number; page: number; rented: boolean; flats: IFlatInfo[] }
> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
    this.state = { pageSize: 20, page: 1, rented: false, flats: [] };
    this.getFlats();
  }

  public render() {
    return (
      <>
        <FlatFilters onPageCountChange={console.log} onShowRentedChange={console.log} />
        {/* <CreateFlatBox /> */}
        {this.getFlatItems()}
      </>
    );
  }

  private openFlat = (id: string) => {
    this.props.history.push(`/flat/${id}`);
  };

  private getFlatItems(): ReactNode[] {
    const flats = this.state.flats.map((flat) => <FlatItem onClick={this.openFlat} key={flat.id} flat={flat} />);
    if (flats.length > 0) {
      return flats;
    } else {
      return [(<Card key={1}>Nuomojamų butų nėra</Card>)]
    }
  }

  private getFlats() {
    FlatService.getFlats(this.state.pageSize, this.state.pageSize * (this.state.page - 1), this.state.rented)
      .then(this.handleFlatResult)
      .catch(this.handleFail);
  }

  private handleFlatResult = (result: IFlatListResponse) => {
    if (result.errors !== undefined) {
      const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
    } else if (result.flats !== undefined) {
      this.setState({ flats: result.flats });
    }
  };

  private handleFail(e: any) {
    toast.error("Įvyko nežinoma klaida.");
  }
}

export default FlatList;
