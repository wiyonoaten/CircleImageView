using Android.App;
using Android.Widget;
using Android.OS;
using HdodenhofCircleImageView;
using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Collections.Generic;

namespace BindingLibTestApp
{
    [Activity(Label = "HdodenhofCircleImageViewBindingLibTestApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ListView listView = FindViewById<ListView>(Resource.Id.list_view);

            var items = new List<Item>();
            for (int i = 0; i < 50; i++)
            {
                items.Add(new Item
                {
                    AvatarResId = Resource.Drawable.me,
                    Name = "Name #" + (i + 1)
                });
            };

            BaseAdapter listAdapter = new ListAdapter(this, new Handler(), items);
            listView.Adapter = listAdapter;
        }

        private class Item : Java.Lang.Object
        {
            public int AvatarResId { get; set; }
            public string Name { get; set; }
        }

        private class ListAdapter : BaseAdapter
        {
            private static readonly IDictionary<int, Bitmap> BITMAP_CACHE = new Dictionary<int, Bitmap>();

            private Context mContext;
            private Handler mUiThreadHandler;
            private IList<Item> mItems;

            public ListAdapter(Context context, Handler uiThreadHandler, IList<Item> items)
            {
                mContext = context;
                mUiThreadHandler = uiThreadHandler;
                mItems = items;
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return mItems[position];
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var item = mItems[position];

                View view = convertView;
                if (view == null)
                {
                    var layoutInflater = mContext.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
                    view = layoutInflater.Inflate(Resource.Layout.ListItem, null);

                    view.Tag = new ViewHolder
                    {
                        imageView = view.FindViewById<CircleImageView>(Resource.Id.image_view),
                        textView = view.FindViewById<TextView>(Resource.Id.text_view)
                    };
                }

                var viewHolder = view.Tag as ViewHolder;

                viewHolder.textView.SetText(item.Name, TextView.BufferType.Normal);

                Bitmap bitmap = null;
                if (BITMAP_CACHE.ContainsKey(item.AvatarResId))
                {
                    bitmap = BITMAP_CACHE[item.AvatarResId];
                }
                else
                {
                    bitmap = BitmapFactory.DecodeResource(mContext.Resources, item.AvatarResId);
					BITMAP_CACHE[item.AvatarResId] = bitmap;
                }
                viewHolder.imageView.SetImageDrawable(new BitmapDrawable(bitmap));

                return view;
            }

            public override int Count
            {
                get { return mItems.Count; }
            }

            private class ViewHolder : Java.Lang.Object
            {
                public CircleImageView imageView { get; set; }
                public TextView textView { get; set; }
            }
        }
    }
}

