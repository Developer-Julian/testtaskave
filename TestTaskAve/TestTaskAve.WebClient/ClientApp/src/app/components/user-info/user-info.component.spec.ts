import { NO_ERRORS_SCHEMA } from "@angular/core";
import { UserInfoComponent } from "./user-info.component";
import { ComponentFixture, TestBed } from "@angular/core/testing";

describe("UserInfoComponent", () => {

  let fixture: ComponentFixture<UserInfoComponent>;
  let component: UserInfoComponent;
  beforeEach(() => {
    TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
      ],
      declarations: [UserInfoComponent]
    });

    fixture = TestBed.createComponent(UserInfoComponent);
    component = fixture.componentInstance;

  });

  it("should be able to create component instance", () => {
    expect(component).toBeDefined();
  });
  
});
