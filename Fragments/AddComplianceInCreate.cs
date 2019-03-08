using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Fragments
{
    [Activity(Theme = "@android:style/Theme.Dialog")]
    public class AddComplianceInCreate : BottomSheetDialogFragment
    {
        RadioButton checkBox1, checkBox2;
        Spinner spinnertype, spinnerextension;
        string compliancetype="";
        int max_numbers=0;
        string filetype, file_format;
        Button Addtolist;
        public static ScrollableListView complianceGridview;
        EditText max_number;
        ServiceHelper restservice;
        Geolocation geo;
        string geolocation;
        InternetConnection ic;
        DbHelper db;
        GridForAttachmentCreateReference gridattachmentlist;
        
        string file_type1;
        string file_extensions;
        string selecteditem="";
        string selectedextensions="";

        List<FileTypeModel> filetypegetlist;
        List<FileExtension> fileextgetlist;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            restservice = new ServiceHelper();
            geo = new Geolocation();
            ic = new InternetConnection();
            db = new DbHelper();
            filetypegetlist = new List<FileTypeModel>();
            fileextgetlist = new List<FileExtension>();
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.addcompliance_createlayout, null);
            checkBox1 = view.FindViewById<RadioButton>(Resource.Id.mandatory);
            checkBox2 = view.FindViewById<RadioButton>(Resource.Id.not);
            spinnerextension = view.FindViewById<Spinner>(Resource.Id.spiner_format);
            spinnertype = view.FindViewById<Spinner>(Resource.Id.spinner_type);
            Addtolist = view.FindViewById<Button>(Resource.Id.btn_addtolist);
            complianceGridview = view.FindViewById<ScrollableListView>(Resource.Id.grid_compliance);
            max_number = view.FindViewById<EditText>(Resource.Id.maxnumberedit);
           
            // modelsaddcompliance = new List<ComplianceJoinTable>();
            checkBox1.Click += RadioButtonClick;
            checkBox2.Click += RadioButtonClick;

            getfiletypemethodAsync();

            if (CreateTaskFrag.modelsaddcompliance.Count > 0)
            {
                gridattachmentlist = new GridForAttachmentCreateReference(Activity, CreateTaskFrag.modelsaddcompliance);
                complianceGridview.Adapter = gridattachmentlist;
                gridattachmentlist.NotifyDataSetChanged();
            }

            Addtolist.Click += delegate
            {
               addtolistcompliance();
            };

            return view;
        }

        private void Spinnertype_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            if (e.Position > 0)
            {
                selecteditem = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
                getextensionmethodAsync(selecteditem);
            }
            
        }

        private void Spinnertype_ItemSelected2(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            if (e.Position > 0)
            {
                selectedextensions = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            }
        }

        private void addtolistcompliance()
        {
            if (!max_number.Text.Equals(""))
            {
                max_numbers = Convert.ToInt32(max_number.Text);
            }
            if (compliancetype.Equals(""))
            {
                Toast.MakeText(Activity,"Please Select ComplianceType", ToastLength.Short).Show();
                return;
            }
            if(selecteditem.Equals(""))
            {
                Toast.MakeText(Activity, "Please Select FileType", ToastLength.Short).Show();
                return;
            }
            if (selectedextensions.Equals(""))
            {
                Toast.MakeText(Activity, "Please Select File Extension", ToastLength.Short).Show();
                return;
            }
            if (max_numbers<=0)
            {
                Toast.MakeText(Activity, "Please Enter maximum number", ToastLength.Short).Show();
                return;
            }
           
            ComplianceJoinTable addtolistcompliace = new ComplianceJoinTable();
            addtolistcompliace.complianceType = compliancetype;
            addtolistcompliace.file_format = selectedextensions;
            addtolistcompliace.file_type = selecteditem;
            addtolistcompliace.max_numbers = max_numbers;

            CreateTaskFrag.modelsaddcompliance.Add(addtolistcompliace);
            gridattachmentlist = new GridForAttachmentCreateReference(Activity, CreateTaskFrag.modelsaddcompliance);
            complianceGridview.Adapter = gridattachmentlist;
            gridattachmentlist.NotifyDataSetChanged();

            //selecteditem = null;
            //selectedextensions = null;
            //max_num = null;
            //compliancetype = null;



        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            compliancetype = rb.Text;
        }
        public async Task getextensionmethodAsync(string filetype)
        {
            try
            {
                if (ic.connectivity())
                {
                    geolocation = geo.GetGeoLocation(Context);
                    string file_extension = await restservice.FileExtension(Activity, filetype, geolocation);
                    List<FileExtension> fileExtensions = JsonConvert.DeserializeObject<List<FileExtension>>(file_extension);
                    db.insertfileextension(fileExtensions);
                    fileextgetlist.Add(new FileExtension {  Text="Select Extension", Value="0"});
                    for(int i = 0; i < fileExtensions.Count; i++)
                    {
                        fileextgetlist.Add(new FileExtension { Text = fileExtensions[i].Text, Value = fileExtensions[i].Value });
                    }
                    spinnerextension.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinnertype_ItemSelected2);
                    ArrayAdapter adapter1 = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleListItem1, fileextgetlist);
                    spinnerextension.Adapter = adapter1;
                }
                else
                {
                    List<FileExtension> fileExtensions = db.getfileextension(filetype);
                    fileextgetlist.Add(new FileExtension { Text = "Select Extension", Value = "0" });
                    for (int i = 0; i < fileExtensions.Count; i++)
                    {
                        fileextgetlist.Add(new FileExtension { Text = fileExtensions[i].Text, Value = fileExtensions[i].Value });
                    }
                    spinnerextension.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinnertype_ItemSelected2);
                    ArrayAdapter adapter1 = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleListItem1, fileextgetlist);
                    spinnerextension.Adapter = adapter1;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async Task getfiletypemethodAsync()
        {
            try
            {
                if (ic.connectivity())
                {
                    geolocation = geo.GetGeoLocation(Context);
                    string filetype = await restservice.FileTypelist(Activity, "", geolocation);
                    List<FileTypeModel> filetypelist = JsonConvert.DeserializeObject<List<FileTypeModel>>(filetype);
                    db.insertfiletype(filetypelist);
                    filetypegetlist.Add(new FileTypeModel {  Document_Type_name="Select Type"});
                    for(int k = 0; k < filetypelist.Count; k++)
                    {
                        filetypegetlist.Add(new FileTypeModel { Document_Type_name = filetypelist[k].Document_Type_name });
                    }
                    spinnertype.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinnertype_ItemSelected);
                    ArrayAdapter adapter = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleListItem1, filetypegetlist);
                    spinnertype.Adapter = adapter;
                }
                else
                {
                    List<FileTypeModel> filetypelist = db.getfiletype();
                    filetypegetlist.Add(new FileTypeModel { Document_Type_name = "Select Type" });
                    for (int k = 0; k < filetypelist.Count; k++)
                    {
                        filetypegetlist.Add(new FileTypeModel { Document_Type_name = filetypelist[k].Document_Type_name });
                    }
                    spinnertype.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinnertype_ItemSelected);
                    ArrayAdapter adapter = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleListItem1, filetypegetlist);
                    spinnertype.Adapter = adapter;
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}